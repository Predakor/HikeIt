import { IconEdit } from "@/Icons/Icons";
import api from "@/Utils/Api/apiRequest";
import { formatDate } from "@/Utils/Formatters";
import { diffObjects } from "@/Utils/objectHelpers";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import {
  DangerButton,
  PrimaryButton,
  SecondaryButton,
} from "@/components/ui/Buttons";
import GoBackButton from "@/components/ui/Buttons/GoBackButton";
import { Dialog } from "@/components/ui/Dialog";
import PageTitle from "@/components/ui/Titles/PageTitle";
import { useTripConfig } from "@/hooks/UseTrips/useTrip";
import { useTripRemove } from "@/hooks/UseTrips/useTripRemove";
import type { TripWithBasicAnalytics } from "@/types/Api/TripDtos";
import { Flex, Heading, Separator, Stack } from "@chakra-ui/react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { useForm } from "react-hook-form";

interface Props {
  trip: TripWithBasicAnalytics;
}

export default function TripDetailsHeader({ trip }: Props) {
  const deleteTrip = useTripRemove();
  const updateTrip = useTripDataMutation(trip.id);
  const [showTripEdit, setShowTripEdit] = useState(false);

  const initialTrip = { tripDay: trip.tripDay, tripName: trip.name };

  const updateTripForm = useForm({
    defaultValues: initialTrip,
  });

  const handleSubmit = updateTripForm.handleSubmit(async (d) => {
    const changes = diffObjects(initialTrip, d);
    const hasChanges = Object.entries(changes).length;
    if (!hasChanges || hasChanges === 0) {
      alert("No changes detected");
      return;
    }
    await updateTrip.mutateAsync(changes);
    setShowTripEdit(false);
  });

  return (
    <>
      <Flex alignItems={"center"} width={"full"} gapX={4}>
        <GoBackButton />
        <Flex flexGrow={1} alignItems={"center"} gapX={8}>
          <PageTitle title={trip.name} />
          <Heading
            display={{ base: "none", lg: "block" }}
            color={"fg.muted"}
            size={{ base: "xl", lg: "2xl" }}
          >
            {formatDate.toUkDate(trip.tripDay)}
          </Heading>
        </Flex>
        <Flex gap={2} align={"center"}>
          <PrimaryButton
            variant={"outline"}
            onClick={() => setShowTripEdit(true)}
          >
            <IconEdit />
            Edit
          </PrimaryButton>
          <DangerButton
            onConfirm={() => deleteTrip.mutate(trip.id)}
            colorPalette={"red"}
            variant={"solid"}
          >
            Delete
          </DangerButton>
        </Flex>
      </Flex>

      <Dialog
        open={showTripEdit}
        title="edit date or name"
        onOpenChange={({ open }) => setShowTripEdit(open)}
      >
        <form onSubmit={handleSubmit}>
          <Stack gap={4}>
            <RenderInputs
              displayOptions={{ size: "lg" }}
              formHook={updateTripForm}
              config={ediTripConfig}
            />
            <Separator />
            <SecondaryButton type="submit">Update</SecondaryButton>
          </Stack>
        </form>
      </Dialog>
    </>
  );
}

const ediTripConfig: InputsConfig = [
  { key: "tripName", label: "", type: "text", min: 3, max: 255 },
  { key: "tripDay", label: "", type: "date", min: 3, max: 255 },
];

function useTripDataMutation(tripId: string) {
  const queryClient = useQueryClient();
  const queryKey = useTripConfig.queryKey(tripId);

  return useMutation({
    mutationKey: ["day"],
    mutationFn: (data: { tripDay?: string; tripName?: string }) =>
      api.patch("trips/" + tripId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey });
    },
  });
}
