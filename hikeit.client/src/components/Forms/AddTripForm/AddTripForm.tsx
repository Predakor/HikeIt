import QueryResult from "@/components/RequestResult/QueryResult";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { regionsList } from "@/data/regionsList";
import { useTripCreate } from "@/hooks/UseTrips/useTripCreate";
import { useTripDraft } from "@/hooks/UseTrips/useTripDraft";
import { Button, Spinner, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
export interface CreateTripForm {
  name: string;
  tripDay: string;
  regionId: number;
}

export type TripFormInitData = Omit<CreateTripForm, "regionId">;

const addTripFormConfig: InputsConfig = [
  { key: "name", label: "Trip name", type: "text", min: 3, max: 63 },
  { key: "tripDay", label: "", type: "date", min: 0, max: Date.now() },
  {
    key: "regionId",
    label: "Region",
    type: "select",
    collection: { items: regionsList, type: "static" },
    required: true,
  },
];

interface Props {
  initData?: TripFormInitData;
  file?: File;
}

function AddTripForm({ initData, file }: Props) {
  const createTrip = useTripCreate();
  const draft = useTripDraft(file);

  const formHook = useForm<CreateTripForm>({
    defaultValues: initData,
  });

  const submitHandler = formHook.handleSubmit(async (data) => {
    const selectedRegion = data.regionId as unknown as string[];

    await draft?.update.mutateAsync({
      tripDay: data.tripDay,
      tripName: data.name,
      regionId: parseInt(selectedRegion[0]),
    });

    await draft?.submit.mutateAsync();
  });

  if (draft?.submit.isPending) {
    return <Spinner />;
  }

  return (
    <Stack asChild>
      <form onSubmit={submitHandler}>
        <QueryResult mutation={draft?.submit} />

        <RenderInputs
          config={addTripFormConfig}
          formHook={formHook}
          displayOptions={{ label: "ontop", size: "lg" }}
        />

        <Button size={"xl"} type="submit">
          Upload
        </Button>
      </form>
    </Stack>
  );
}
export default AddTripForm;
