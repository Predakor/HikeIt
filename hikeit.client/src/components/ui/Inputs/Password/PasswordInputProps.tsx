"use client";

import type { InputProps } from "@chakra-ui/react/input";
import type { PasswordVisibilityProps } from "./password-input";
import type { GroupProps } from "@chakra-ui/react/group";
import type { FieldValues, UseFormRegister } from "react-hook-form";

export interface PasswordInputProps<T extends FieldValues = FieldValues>
  extends InputProps,
    PasswordVisibilityProps {
  rootProps?: GroupProps;
  register?: UseFormRegister<T>;
}
