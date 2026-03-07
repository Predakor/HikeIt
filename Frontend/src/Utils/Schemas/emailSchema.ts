import type { EmailInputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";
import { validators } from "../Validators";

export const emailSchema: EmailInputConfigEntry = {
  key: "email",
  type: "email",
  label: "Email",
  required: true,
  min: 5,
  max: 256,
  validate: validators.email,
};
