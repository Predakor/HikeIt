import RowStat from "@/components/Stats/RowStat";
import SubTitle from "@/components/Titles/SubTitle";
import type { UserBaseProfile } from "@/types/User/user.types";
import { Card, SimpleGrid, Spacer, Stack, Text } from "@chakra-ui/react";
import { UserAvatar } from "../Avatar/UserAvatar";
import { formatter } from "../Stats/Utils/formatter";

export function UserHeaderCard({ user }: { user: UserBaseProfile }) {
  return (
    <Card.Root>
      <Stack
        asChild
        direction={{ base: "column", lg: "row" }}
        gap={4}
        align={"center"}
      >
        <Card.Body>
          <UserAvatar user={user} />

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
            <RowStat label="Trips" value={user.totalTrips} />
            <RowStat label={"Peaks "} value={user.totalPeaks} />
            <RowStat
              label="Travaled"
              value={user.totalDistance}
              addons={{ unit: "km", formatt: formatter.toKm }}
            />
          </SimpleGrid>
        </Card.Body>
      </Stack>
    </Card.Root>
  );
}
