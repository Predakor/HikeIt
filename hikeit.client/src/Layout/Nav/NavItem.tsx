import { preload } from "@/data/routes/routes";
import { LinkBox, Text } from "@chakra-ui/react";
import { useEffect } from "react";
import { NavLink } from "react-router";

type Options = {
  preload?: "onHover" | "onRender" | "none";
};

interface Props {
  path: string;
  label: string;
  options?: Options;
}

const defaultOptions: Options = {
  preload: "onHover",
};

export function NavItem({ path, label, options = defaultOptions }: Props) {
  const hover = {
    color: "fg",
    scale: 1.1,
    transition: "scale 100ms",
  };

  options?.preload ?? "onHover";

  useEffect(() => {
    if (options?.preload === "onRender") {
      preload(path);
    }
  }, []);

  const handlePreload = () => {
    return options?.preload === "onHover" ? preload(path) : undefined;
  };

  return (
    <LinkBox onMouseEnter={handlePreload} fontSize={"inherit"} asChild>
      <NavLink to={path} aria-label={label} key={path}>
        {({ isActive }) => {
          const color = isActive ? "fg" : "fg.muted";
          return (
            <Text
              transition={"color 100ms"}
              fontWeight={"medium"}
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
