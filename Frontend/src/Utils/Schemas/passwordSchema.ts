import type { PasswordInputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";
import { validators } from "../Validators";

export const passwordSchema: PasswordInputConfigEntry = {
  key: "password",
  type: "password",
  label: "",
  min: 6,
  max: 64,
  required: true,
  validate: validators.password,
};
