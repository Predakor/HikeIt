import QueryResult from "@/components/ui/Results/QueryResult";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { useTripDraft } from "@/hooks/UseTrips/useTripDraft";
import { Button, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { TripAddedSucces } from "./TripAddedSucces";
import { addTripFormConfig } from "./addTripFormConfig";
export interface CreateTripForm {
  name: string;
  tripDay: string;
  regionId: number;
}

export type TripFormInitData = Omit<CreateTripForm, "regionId">;

interface Props {
  initData?: TripFormInitData;
  file?: File;
  resetForm: () => void;
}

function AddTripForm({ initData, file, resetForm }: Props) {
  const draft = useTripDraft(file);

  const formHook = useForm<CreateTripForm>({
    defaultValues: initData,
  });

  const submitHandler = formHook.handleSubmit(async (data) => {
    if (draft?.submit.isPending) {
      return;
    }

    const selectedRegion = data.regionId as unknown as string[];

    await draft?.update.mutateAsync({
      tripDay: data.tripDay,
      tripName: data.name,
      regionId: parseInt(selectedRegion[0]),
    });

    await draft?.submit.mutateAsync();
  });

  return (
    <Stack>
      {draft?.submit.isSuccess && (
        <TripAddedSucces
          tripLocation={draft.submit.data.location}
          resetForm={resetForm}
        />
      )}

      {!draft?.submit.data && (
        <form onSubmit={submitHandler}>
          <Stack>
            <QueryResult mutation={draft?.submit} />

            <RenderInputs
              config={addTripFormConfig}
              formHook={formHook}
              displayOptions={{ label: "ontop", size: "xl" }}
            />

            <Button
              loading={draft?.submit.isPending}
              loadingText="Uploading"
              colorPalette={"blue"}
              size={"xl"}
              marginTop={2}
              type="submit"
            >
              Upload
            </Button>
          </Stack>
        </form>
      )}
    </Stack>
  );
}

export default AddTripForm;
