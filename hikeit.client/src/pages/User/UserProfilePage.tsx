import api from "@/Utils/Api/apiRequest";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import { Box, Heading, Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

function UserProfilePage() {
  const request = useQuery({
    queryKey: ["profile"],
    queryFn: () => api.get("users/profile"),
  });

  console.log(request.data);

  return (
    <Stack>
      <FetchWrapper request={request}>
        {(data) =>
          ObjectToArray(data as any).map(([key, value]) => (
            <ObjectEntry oKey={key as string} value={value} />
          ))
        }
      </FetchWrapper>
    </Stack>
  );
}

function ObjectEntry({ oKey: key, value }: { oKey: string; value: any }) {
  return (
    <Box>
      <Heading> {key}</Heading>
      {value}
    </Box>
  );
}

export default UserProfilePage;
