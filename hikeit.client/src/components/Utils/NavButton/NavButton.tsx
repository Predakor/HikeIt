import { Link, LinkBox } from "@chakra-ui/react";
import { NavLink } from "react-router";

interface Props {
  to: string;
  label: string;
}

export default function NavButton({ to, label }: Props) {
  return (
    <LinkBox fontSize={"lg"} asChild>
      <Link asChild>
        <NavLink aria-label={label} to={to}>
          {label}
        </NavLink>
      </Link>
    </LinkBox>
  );
}
