import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import type { RegisterForm } from "@/hooks/useAuth";
import { useForm } from "react-hook-form";

const registerFormConfgig: InputsConfig = [
  {
    key: "userName",
    label: "",
    type: "text",
    min: 3,
    max: 64,
    required: true,
  },
  {
    key: "email",
    label: "",
    type: "text",
    min: 3,
    max: 64,
    required: true,
  },
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
  {
    key: "password",
    label: "",
    type: "password",
    min: 6,
    max: 64,
    pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).*$/,
    required: true,
  },
];

export default function useRegister() {
  const hookForm = useForm<RegisterForm>();

  return [hookForm, registerFormConfgig] as const;
}
