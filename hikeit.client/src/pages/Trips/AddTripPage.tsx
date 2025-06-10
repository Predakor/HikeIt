import GpxArrayBuilder from "@/Utils/Builders/GpxArrayBuilder";
import DropFile from "@/components/AddTripForm/AddFile/DropFile";
import AddTripPresenter from "@/components/AddTripForm/AddTripPresenter";
import Divider from "@/components/Divider/Divider";
import usePost from "@/hooks/usePost";
import type { CreateTrip } from "@/types/ApiTypes/TripDtos";
import type { FullMap, PartialMap } from "@/types/Utils/MappingTypes";
import {
  Alert,
  Box,
  Button,
  Center,
  Heading,
  ProgressCircle,
  Stack,
} from "@chakra-ui/react";
import { useRef, type FormEvent } from "react";
import { useForm } from "react-hook-form";

const defaultValues: CreateTrip = {
  base: {
    name: "",
    tripDay: "",
  },
  regionId: 1,
};

const formInputs: PartialMap<
  CreateTrip,
  {
    type: number | string | Date;
  }
> = {};

function AddTripPage() {
  const fileRef = useRef<File | null>(null);

  const [post, result] = usePost();
  const formHandler = useForm<CreateTrip>({
    defaultValues,
  });

  const submitHandler = async (e: FormEvent) => {
    e.preventDefault();
    const data = formHandler.getValues();
    const file = fileRef.current;

    if (!file) {
      alert("Please provide a .gpx file.");
      return;
    }

    const formData = new FormData();

    formData.append("Base.Name", data.base.name);
    formData.append("RegionId", data.regionId.toString());
    formData.append("Base.TripDay", data.base.tripDay);
    formData.append("file", file);

    console.log({ formData, formHandler });

    await post("trips/form", formData);
  };

  const fileChangeHandler = (newFile: File) => {
    const mapFromFile = async () => {
      const fileData = await GpxArrayBuilder.fromFile(newFile);
      const stats = fileData.smoothMedian().generateGains().getStats();

      const tripDate = stats.startTime?.slice(0, 10) || "";

      formHandler.setValue("base.tripDay", tripDate);
    };

    fileRef.current = newFile;
    mapFromFile();
  };

  if (result.pending) {
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
    <Stack alignItems={"center"} w={"100%"} gap={10}>
      <Center>
        <Heading size={"5xl"}>Add Your Trip</Heading>
      </Center>

      {result.result && (
        <Alert.Root
          status={result.result === typeof Error ? "error" : "neutral"}
        >
          <Alert.Indicator />
          <Alert.Content>
            <Alert.Title>{result.result.toString()}</Alert.Title>
            <Alert.Description>
              Your form has some errors. Please fix them and try again.
            </Alert.Description>
          </Alert.Content>
        </Alert.Root>
      )}

      <Stack as={"form"} onSubmit={submitHandler} gap={"2em"}>
        <Stack
          alignItems={"start"}
          direction={{ base: "column", md: "row" }}
          gap={{ md: "5em" }}
          flex={1}
        >
          <Box>
            <Heading fontSize={"2xl"} py={"1em"}>
              Manually
            </Heading>
            <AddTripPresenter formHandler={formHandler} />
          </Box>

          <Divider />

          <Box>
            <Heading fontSize={"2xl"} py={"1em"}>
              From a gpx file
            </Heading>
            <DropFile
              allowedFiles={[".gpx"]}
              onFileChange={fileChangeHandler}
            />
          </Box>
        </Stack>
        <Button type="submit">Upload</Button>
      </Stack>
    </Stack>
  );
}
export default AddTripPage;
