import { isNumber } from "@/Utils/numberUtils";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { Button, Field, Input, Stack } from "@chakra-ui/react";
import type { ChangeEvent } from "react";
import { useForm, type UseFormSetValue } from "react-hook-form";

export type AddPeakConfig = {
  name: string;
  height: number;
  latitude: number;
  longitude: number;
  regionId?: number;
};

interface Props {
  onSubmit: (data: AddPeakConfig) => void;
}

export default function AddPeakForm({ onSubmit }: Props) {
  const formHook = useForm<AddPeakConfig>();

  const handleSubmit = formHook.handleSubmit((d) => {
    onSubmit(d);
  });

  return (
    <Stack direction={{ base: "column", lg: "row" }}>
      <form onSubmit={handleSubmit}>
        <Stack gapY={4}>
          <RenderInputs
            displayOptions={{ size: "xl" }}
            config={addPeakFormConfig}
            formHook={formHook}
          />

          <Button colorPalette={"blue"} size={"xl"} type="submit">
            Upload
          </Button>
        </Stack>
      </form>
      <FormHelpers setForm={formHook.setValue} />
    </Stack>
  );
}

interface Props2 {
  setForm: UseFormSetValue<AddPeakConfig>;
}

function FormHelpers({ setForm }: Props2) {
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
      setForm("name", strings.join(""));
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

const addPeakFormConfig: InputsConfig = [
  {
    key: "name",
    label: "",
    type: "text",
    min: 3,
    max: 254,
    validate: (value) => {
      if (value.length < 3) return "is too short";
      if (value.length > 254) return "is too loong";
      return true;
    },
  },
  {
    key: "height",
    label: "",
    type: "number",
    min: 1,
    max: 8849,
    required: true,
  },
  {
    key: "latitude",
    label: "",
    type: "number",
    step: "0.001",
    min: 0,
    max: 180,
    required: true,
  },
  {
    key: "longitude",
    label: "",
    type: "number",
    step: "0.001",
    min: 0,
    max: 90,
    required: true,
  },
];
