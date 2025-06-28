import GpxArrayBuilder from "@/Utils/Builders/GpxArrayBuilder";
import DropFile from "@/components/AddTripForm/AddFile/DropFile";
import Divider from "@/components/Divider/Divider";
import QueryResult from "@/components/RequestResult/QueryREsult";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { regionsList } from "@/data/regionsList";
import { useTripCreate } from "@/hooks/useTrips";
import {
  Alert,
  Box,
  Button,
  Center,
  Heading,
  LinkBox,
  ProgressCircle,
  Stack,
} from "@chakra-ui/react";
import { useRef, useState } from "react";
import { useForm } from "react-hook-form";
import { Link } from "react-router";

interface CreateTripForm {
  name: string;
  tripDay: string;
  regionId: number;
}

const addTripFormConfig: InputsConfig = [
  { key: "name", label: "Trip name", type: "text", min: 3, max: 63 },
  { key: "tripDay", label: "", type: "date", min: 0, max: Date.now() },
  {
    key: "regionId",
    label: "Region",
    type: "select",
    collection: { items: regionsList, type: "static" },
  },
];

function AddTripPage() {
  const fileRef = useRef<File | null>(null);

  const createTrip = useTripCreate();

  const { handleSubmit, control, register, setValue } =
    useForm<CreateTripForm>();

  const submitHandler = handleSubmit(async (data) => {
    const file = fileRef.current;

    if (!file) {
      alert("Please provide a .gpx file.");
      return;
    }
    const formData = new FormData();

    formData.append("Base.Name", data.name);
    formData.append("RegionId", data.regionId.toString());
    formData.append("Base.TripDay", data.tripDay);
    formData.append("file", file);

    createTrip.mutate(formData);
  });

  const fileChangeHandler = (newFile: File) => {
    const mapFromFile = async () => {
      const fileData = await GpxArrayBuilder.fromFile(newFile);
      const stats = fileData.smoothMedian().generateGains().getStats();

      const tripDate = stats.startTime?.slice(0, 10) || "";

      if (tripDate) {
        setValue("tripDay", tripDate);
      }
    };

    fileRef.current = newFile;
    mapFromFile();
  };

  if (createTrip.isPending) {
    return (
      <ProgressCircle.Root value={null} size="sm">
        <ProgressCircle.Circle>
          <ProgressCircle.Track />
          <ProgressCircle.Range />
        </ProgressCircle.Circle>
      </ProgressCircle.Root>
    );
  }

  return (
    <Stack alignItems={{ base: "", lg: "center" }} gapY={8}>
      <Heading size={"5xl"}>Add Your Trip</Heading>

      <Stack as={"form"} onSubmit={submitHandler} gapY={8}>
        <QueryResult mutation={createTrip} />
        <Stack
          direction={{ base: "column", md: "row" }}
          align={{ base: "stretch", lg: "center" }}
          gap={{ md: "5em" }}
          flex={1}
        >
          <Box>
            <Heading fontSize={"2xl"} py={"1em"}>
              Manually
            </Heading>
            <RenderInputs
              config={addTripFormConfig}
              control={control}
              register={register}
              displayOptions={{ label: "ontop", size: "lg" }}
            />
          </Box>
          <Divider />
          <Box>
            <Heading fontSize={"2xl"} py={"1em"}>
              From a gpx file
            </Heading>
            <Stack>
              <DropFile
                allowedFiles={[".gpx"]}
                onFileChange={fileChangeHandler}
              />
            </Stack>
          </Box>
        </Stack>
        <Button size={"xl"} type="submit">
          Upload
        </Button>
      </Stack>
    </Stack>
  );
}
export default AddTripPage;
