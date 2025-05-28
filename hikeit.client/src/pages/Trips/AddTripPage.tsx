import GpxArrayBuilder from "@/Utils/Builders/GpxArrayBuilder";
import DropFile from "@/components/AddTripForm/AddFile/DropFile";
import type { TripDto } from "@/components/AddTripForm/AddTrip/types";
import AddTripForm from "@/components/AddTripForm/AddTripForm";
import Divider from "@/components/Divider/Divider";
import { Box, Button, Center, Heading, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";

function AddTripPage() {
  let file: File;
  const formHandler = useForm<TripDto>({
    defaultValues: {
      height: 0,
      distance: 0,
      duration: 0,
      regionId: 0,
      tripDay: "",
    },
  });

  const submitHandler = () => {};

  const fileChangeHandler = (newFile: File) => {
    const mapFromFile = async () => {
      const fileData = await GpxArrayBuilder.fromFile(newFile);
      const stats = fileData.smoothMedian().generateGains().getStats();

      formHandler.setValue("height", stats.climbed);
      formHandler.setValue("distance", stats.distance);

      formHandler.setValue("duration", stats?.duration || 0);
      formHandler.setValue("tripDay", stats.startTime?.slice(0, 10) || "");
    };

    file = newFile;
    mapFromFile();
  };

  return (
    <Stack alignItems={"center"} w={"100%"} gap={10}>
      <Center>
        <Heading size={"5xl"}>Add Your Trip</Heading>
      </Center>

      <Stack as={"form"} gap={"2em"}>
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
            <AddTripForm formHandler={formHandler} />
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
        <Button onClick={submitHandler}>Upload</Button>
      </Stack>
    </Stack>
  );
}
export default AddTripPage;
