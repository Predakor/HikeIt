import GpxArrayBuilder from "@/Utils/Builders/GpxArray";
import GoBackButton from "@/components/ui/Buttons/GoBackButton";
import AddTripForm, { type TripFormInitData } from "@/components/Forms/AddTripForm/AddTripForm";
import PageTitle from "@/components/ui/Titles/PageTitle";
import AddTripInstructions from "@/components/Trip/AddTripInfo/AddTripInfo";
import { Flex, Stack } from "@chakra-ui/react";
import { useState } from "react";

type SelectedTrip = { file: File; initData: TripFormInitData };

function AddTripPage() {
  const [selected, setSelected] = useState<SelectedTrip>();

  const fileChangeHandler = async (newFile: File) => {
    const fileData = await GpxArrayBuilder.fromFile(newFile);
    const stats = fileData.smoothMedian().generateGains().getStats();
    const tripDate = stats.startTime?.slice(0, 10) || "";

    const fileName = newFile.name.split(".");

    setSelected({
      file: newFile,
      initData: {
        name: fileName.length > 1 ? fileName.slice(0, -1).join(".") : fileName[0],
        tripDay: tripDate,
      },
    });
  };

  return (
    <Stack gapY={{ base: 8, lg: 16 }}>
      <Flex gap={4}>
        <GoBackButton />
        <PageTitle title="Add Your Trip" />
      </Flex>

      <Stack alignSelf={"center"} gapY={12}>
        {selected ?
          <AddTripForm
            initData={selected.initData}
            file={selected.file}
            resetForm={() => setSelected(undefined)}
          />
        : <AddTripInstructions fileChangeHandler={fileChangeHandler} />}
      </Stack>
    </Stack>
  );
}

export default AddTripPage;
