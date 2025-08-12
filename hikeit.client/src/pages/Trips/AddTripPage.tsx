import GpxArrayBuilder from "@/Utils/Builders/GpxArrayBuilder";
import GoBackButton from "@/components/ui/Buttons/GoBackButton";
import AddTripForm, {
  type TripFormInitData,
} from "@/components/Forms/AddTripForm/AddTripForm";
import PageTitle from "@/components/Titles/PageTitle";
import AddTripInstructions from "@/components/Trip/AddTripInfo/AddTripInfo";
import { Flex, Stack } from "@chakra-ui/react";
import { useRef, useState } from "react";

function AddTripPage() {
  const fileRef = useRef<File>(undefined);
  const initStatsRef = useRef<TripFormInitData>({
    name: "",
    tripDay: "",
  });
  const [showForm, setShowForm] = useState(false);

  const fileChangeHandler = async (newFile: File) => {
    const fileData = await GpxArrayBuilder.fromFile(newFile);
    const stats = fileData.smoothMedian().generateGains().getStats();
    const tripDate = stats.startTime?.slice(0, 10) || "";

    const initStats = initStatsRef.current;

    const fileName = newFile.name.split(".");
    initStats.name =
      fileName.length > 1 ? fileName.slice(0, -1).join(".") : fileName[0];

    if (tripDate) {
      initStats.tripDay = tripDate;
    }

    initStatsRef.current = initStats;
    fileRef.current = newFile;
    setShowForm(true);
  };

  return (
    <Stack gapY={{ base: 8, lg: 16 }}>
      <Flex gap={4}>
        <GoBackButton />
        <PageTitle title="Add Your Trip" />
      </Flex>

      <Stack alignSelf={"center"} gapY={12}>
        {showForm ? (
          <AddTripForm
            initData={initStatsRef.current}
            file={fileRef.current}
            resetForm={() => setShowForm(false)}
          />
        ) : (
          <AddTripInstructions fileChangeHandler={fileChangeHandler} />
        )}
      </Stack>
    </Stack>
  );
}

export default AddTripPage;
