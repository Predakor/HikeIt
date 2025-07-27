import type { EmailInputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";

export const emailSchema: EmailInputConfigEntry = {
  key: "email",
  type: "email",
  label: "Email",
  required: true,
  pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
  min: 5,
  max: 256,
};
