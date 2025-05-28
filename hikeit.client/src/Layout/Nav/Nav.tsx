import { routes } from "@/data/routes";
import { Flex, Text } from "@chakra-ui/react";
import { NavLink } from "react-router";

function Nav() {
  const visibleRoutes = routes.filter((route) => !route.hidden);

  return (
    <nav>
      <Flex gap={20}>
        {visibleRoutes.map(({ path, label }) => (
          <NavLink to={path} aria-label={label} key={path}>
            {({ isActive }) => {
              const size = isActive ? "2xl" : "lg";
              return <Text textStyle={size}>{label}</Text>;
            }}
          </NavLink>
        ))}
      </Flex>
    </nav>
  );
}

export default Nav;
