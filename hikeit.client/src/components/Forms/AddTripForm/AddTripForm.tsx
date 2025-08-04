import QueryResult from "@/components/RequestResult/QueryResult";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { regionsList } from "@/data/regionsList";
import { useTripCreate } from "@/hooks/UseTrips/useTripCreate";
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

  const formHook = useForm<CreateTripForm>({
    defaultValues: initData,
  });

  const submitHandler = formHook.handleSubmit(async (data) => {
    const formData = new FormData();

    formData.append("Base.Name", data.name);
    formData.append("RegionId", data.regionId.toString());
    formData.append("Base.TripDay", data.tripDay);

    if (file) {
      formData.append("file", file);
    }

    createTrip.mutate(formData);
  });

  if (createTrip.isPending) {
    return <Spinner />;
  }

  console.log(createTrip);

  return (
    <Stack asChild>
      <form onSubmit={submitHandler}>
        <QueryResult mutation={createTrip} />

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
