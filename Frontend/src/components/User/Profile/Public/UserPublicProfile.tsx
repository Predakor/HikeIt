import RowStat from "@/components/Stats/RowStat";
import SubTitle from "@/components/ui/Titles/SubTitle";
import type { UserBaseProfile } from "@/types/User/user.types";
import { Card, SimpleGrid, Spacer, Stack, Text } from "@chakra-ui/react";
import EditableAvatar from "../../Avatar/EditableAvatar";
import { formatter } from "../../Stats/Utils/formatter";

export function UserPublicProfile({ user }: { user: UserBaseProfile }) {
  return (
    <Card.Root>
      <Stack
        asChild
        direction={{ base: "column", lg: "row" }}
        gap={4}
        align={"center"}
      >
        <Card.Body>
          <EditableAvatar user={user} />

          <Stack>
            <SubTitle
              title={user.userName}
              textAlign={{ base: "center", lg: "start" }}
              fontSize={"4xl"}
              fontWeight={"black"}
            />
            <Text fontSize={"xl"}>{user.rank}</Text>
          </Stack>

          <Spacer />
          <SimpleGrid gap={12} gridAutoFlow={"column"}>
            <RowStat label="Trips" value={user.trips} />
            <RowStat label={"Peaks "} value={user.peaks} />
            <RowStat
              label="Travaled"
              value={user.traveled}
              addons={{ unit: "km", formatt: formatter.toKm }}
            />
          </SimpleGrid>
        </Card.Body>
      </Stack>
    </Card.Root>
  );
}
