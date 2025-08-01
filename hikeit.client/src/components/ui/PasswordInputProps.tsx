"use client";
import type { GroupProps, InputProps } from "@chakra-ui/react";
import type { FieldValues, UseFormRegister } from "react-hook-form";
import type { PasswordVisibilityProps } from "./password-input";

export interface PasswordInputProps<T extends FieldValues = FieldValues>
  extends InputProps,
    PasswordVisibilityProps {
  rootProps?: GroupProps;
  register?: UseFormRegister<T>;
}
