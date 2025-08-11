import { isNumber } from "@/Utils/numberUtils";
import { Field, Input, Stack } from "@chakra-ui/react";
import { type ChangeEvent } from "react";
import { type UseFormSetValue } from "react-hook-form";
import type { AddPeakConfig } from "./addPeakFormConfig";

interface Props {
  setForm: UseFormSetValue<AddPeakConfig>;
}

export function FormHelpers({ setForm }: Props) {
  const extractCoordinates = (e: ChangeEvent<HTMLInputElement>) => {
    const location = e.currentTarget.value;
    const [lat, lon] = location
      .split(",")
      .map((v) => Number(v).toFixed(3))
      .map((v) => Number(v));

    setForm("latitude", lat);
    setForm("longitude", lon);
  };

  const extractNameAndHeight = (e: ChangeEvent<HTMLInputElement>) => {
    const words = e.currentTarget.value.split(" ");

    if (words.length === 1) {
      const word = words[1];
      if (isNumber(word)) setForm("height", Number(word));
      else setForm("name", word);
    }

    const strings = words.filter((w) => !isNumber(w));
    const numbers = words.filter((w) => isNumber(w)).map((w) => Number(w));

    if (strings.length > 0) {
      setForm("name", strings.join(" "));
    }

    if (numbers.length > 0) {
      setForm("height", numbers[0]);
    }
  };

  return (
    <Stack gapY={4}>
      <Field.Root>
        <Field.Label>Paste name with height</Field.Label>
        <Input
          type="text"
          size={"xl"}
          placeholder="sniezka 1655m"
          onChange={extractNameAndHeight}
        />
      </Field.Root>

      <Field.Root>
        <Field.Label>Location string</Field.Label>

        <Input
          type="text"
          size={"xl"}
          placeholder="50.83694368490069, 15.682658760063202"
          onChange={extractCoordinates}
        />
      </Field.Root>
    </Stack>
  );
}
