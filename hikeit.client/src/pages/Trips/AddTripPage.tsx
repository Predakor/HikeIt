import GpxArrayBuilder from "@/Utils/Builders/GpxArrayBuilder";
import DropFile from "@/components/AddTripForm/AddFile/DropFile";
import AddTripPresenter from "@/components/AddTripForm/AddTripPresenter";
import Divider from "@/components/Divider/Divider";
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
import { useRef, type FormEvent } from "react";
import { useForm } from "react-hook-form";

const defaultValues = {
  height: 0,
  distance: 0,
  duration: 0,
  regionId: 0,
  tripDay: "",
};

type FormData = {
  height: number;
  distance: number;
  duration: number;
  regionId: number;
  tripDay: string;
};

function AddTripPage() {
  const fileRef = useRef<File | null>(null);

  const [post, result] = usePost();
  const formHandler = useForm<FormData>({
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

    formData.append("RegionId", data.regionId.toString());
    formData.append("Base.Height", data.height.toString());
    formData.append("Base.Distance", data.distance.toString());
    formData.append("Base.Duration", data.duration.toString());
    formData.append("Base.TripDay", data.tripDay);
    formData.append("file", file);

    console.log({ formData, formHandler });

    await post("trips/form", formData);
  };

  const fileChangeHandler = (newFile: File) => {
    const mapFromFile = async () => {
      const fileData = await GpxArrayBuilder.fromFile(newFile);
      const stats = fileData.smoothMedian().generateGains().getStats();

      formHandler.setValue("height", stats.climbed);
      formHandler.setValue("distance", stats.distance);

      const duration = stats.duration ? parseInt(stats.duration.toFixed(1)) : 0;
      const tripDate = stats.startTime?.slice(0, 10) || "";

      formHandler.setValue("duration", duration);
      formHandler.setValue("tripDay", tripDate);
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
