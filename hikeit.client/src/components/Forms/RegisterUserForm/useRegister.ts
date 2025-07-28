import schemas from "@/Utils/Schemas";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import type { RegisterForm } from "@/hooks/useAuth";
import { useForm } from "react-hook-form";

const registerFormConfgig: InputsConfig = [
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

export default function useRegister() {
  const hookForm = useForm<RegisterForm>();

  return [hookForm, registerFormConfgig] as const;
}
