import type { TextInputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";
import { validators } from "../Validators";

export const loginSchema: TextInputConfigEntry = {
  key: "userName",
  type: "text",
  label: "",
  min: 3,
  max: 63,
  required: true,
  validate: validators.login,
};
