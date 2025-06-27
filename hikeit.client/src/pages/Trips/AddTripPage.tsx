import GpxArrayBuilder from "@/Utils/Builders/GpxArrayBuilder";
import DropFile from "@/components/AddTripForm/AddFile/DropFile";
import Divider from "@/components/Divider/Divider";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { regionsList } from "@/data/regionsList";
import usePost from "@/hooks/usePost";
import {
  Alert,
  Box,
  Button,
  Center,
  Heading,
  ProgressCircle,
  Stack,
} from "@chakra-ui/react";
import { useRef } from "react";
import { useForm } from "react-hook-form";

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

  const [post, result] = usePost();

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
    console.log(data);

    await post("trips/form", formData);
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
          <Alert.Content
            onClick={() => {
              console.log(result.result);
              result.result;
            }}
          >
            <Alert.Title>{result.result.toString()}</Alert.Title>
            <Alert.Description>Trip Created succesfully</Alert.Description>
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
            <Stack>
              <RenderInputs
                config={addTripFormConfig}
                control={control}
                register={register}
                displayOptions={{ label: "ontop", size: "lg" }}
              />
            </Stack>
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
