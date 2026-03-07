import { Flex, Link, Show, type LinkProps } from "@chakra-ui/react";
import type { ReactNode } from "react";

interface Props extends Partial<LinkProps> {
  label: string;
  icon: ReactNode;
}

function ExternalLink(props: Props) {
  const { icon, label, ...rest } = props;
  return (
    <Link {...rest} rel="noopener noreferrer" target="_blank">
      <Flex alignItems={"center"} gap={2}>
        {label}
        <Show when={icon}>{icon}</Show>
      </Flex>
    </Link>
  );
}
export default ExternalLink;
