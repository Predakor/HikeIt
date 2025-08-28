import schemas from "@/Utils/Schemas";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";

export const registerFormConfgig: InputsConfig = [
  schemas.login,
  schemas.email,
  {
    key: "firstName",
    label: "",
    type: "text",
    min: 3,
    max: 64,
    required: true,
  },
  {
    key: "lastName",
    label: "",
    type: "text",
    min: 3,
    max: 64,
    required: true,
  },
  schemas.password,
];
