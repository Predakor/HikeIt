import GpxArrayBuilder from "@/Utils/Builders/GpxArrayBuilder";
import DropFile from "@/components/AddTripForm/AddFile/DropFile";
import type { TripDto } from "@/components/AddTripForm/AddTrip/types";
import AddTripPresenter from "@/components/AddTripForm/AddTripPresenter";
import Divider from "@/components/Divider/Divider";
import usePost from "@/hooks/usePost";
import { Box, Button, Center, Heading, Stack } from "@chakra-ui/react";
import { useRef, type FormEvent } from "react";
import { useForm } from "react-hook-form";

const defaultValues = {
  height: 0,
  distance: 0,
  duration: 0,
  regionId: 0,
  tripDay: "",
};

function AddTripPage() {
  const fileRef = useRef<File | null>(null);
  const [post, result] = usePost();
  const formHandler = useForm<TripDto>({
    defaultValues,
  });

  const file = fileRef.current;

  const submitHandler = (e: FormEvent) => {
    e.preventDefault();
    const data = formHandler.getValues();
    post("trips", JSON.stringify(data));

    if (file) {
      const formData = new FormData();
      formData.append("file", file);
      post("files", formData);
    }
  };

  const fileChangeHandler = (newFile: File) => {
    const mapFromFile = async () => {
      const fileData = await GpxArrayBuilder.fromFile(newFile);
      const stats = fileData.smoothMedian().generateGains().getStats();

      formHandler.setValue("height", stats.climbed);
      formHandler.setValue("distance", stats.distance);

      formHandler.setValue("duration", stats?.duration || 0);
      formHandler.setValue("tripDay", stats.startTime?.slice(0, 10) || "");
    };

    fileRef.current = newFile;
    mapFromFile();
  };

  console.log(result);

  return (
    <Stack alignItems={"center"} w={"100%"} gap={10}>
      <Center>
        <Heading size={"5xl"}>Add Your Trip</Heading>
      </Center>

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
