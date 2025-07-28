"use client";
import type { GroupProps, InputProps } from "@chakra-ui/react";
import type { UseFormRegister } from "react-hook-form";
import { PasswordVisibilityProps } from "./password-input";

export interface PasswordInputProps<T, FieldValues>
  extends InputProps,
    PasswordVisibilityProps {
  rootProps?: GroupProps;
  register?: UseFormRegister<T>;
}
