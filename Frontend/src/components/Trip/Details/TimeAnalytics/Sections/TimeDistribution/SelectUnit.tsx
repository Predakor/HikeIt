import { RadioButtons } from "@/components/ui/Buttons/RadioButtons";
import type { Action } from "@/types/Utils/func.types";
import { ButtonGroup } from "@chakra-ui/react";
import type { AllowedUnit } from "./TimeDistributionSection";

export function SelectUnit(props: {
  onChange: Action<AllowedUnit>;
  selectedItem: AllowedUnit;
}) {
  return (
    <ButtonGroup>
      <RadioButtons
        size={"xs"}
        items={[
          { label: "h", value: "hours" },
          { label: "m", value: "minutes" },
          { label: "%", value: "percentage" },
        ]}
        {...props}
      />
    </ButtonGroup>
  );
}
