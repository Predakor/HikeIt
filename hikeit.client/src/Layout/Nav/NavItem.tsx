import { LinkBox, Text } from "@chakra-ui/react";
import { NavLink } from "react-router";

export function NavItem({ path, label }: { path: string; label: string }) {
  const hover = {
    color: "fg",
    scale: 1.1,
    transition: "scale 100ms",
  };

  return (
    <LinkBox fontSize={"inherit"} asChild>
      <NavLink to={path} aria-label={label} key={path}>
        {({ isActive }) => {
          const color = isActive ? "fg" : "fg.muted";
          return (
            <Text
              transition={"color 100ms"}
              scale={isActive ? 1.1 : 1}
              color={color}
              _hover={hover}
            >
              {label}
            </Text>
          );
        }}
      </NavLink>
    </LinkBox>
  );
}
