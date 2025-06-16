import { routes } from "@/data/routes";
import { LinkBox, Text } from "@chakra-ui/react";
import { NavLink } from "react-router";

function NavContent() {
  const visibleRoutes = routes.filter((route) => !route.hidden);

  return visibleRoutes.map(({ path, label }) => (
    <LinkBox fontSize={"xl"} asChild>
      <NavLink to={path} aria-label={label} key={path}>
        {({ isActive }) => {
          const color = isActive ? "fg" : "fg.muted";
          return (
            <Text
              transition={"color 100ms"}
              scale={isActive ? 1.1 : 1}
              color={color}
              _hover={{
                color: "fg",
                scale: 1.1,
                transition: "scale 100ms",
              }}
            >
              {label}
            </Text>
          );
        }}
      </NavLink>
    </LinkBox>
  ));
}

export default NavContent;
