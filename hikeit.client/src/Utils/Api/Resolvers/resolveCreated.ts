export const resolveCreated = async (response: Response) => {
  if (!response.ok) {
    throw await MapToError(response);
  }

  const res = {
    location: "",
  };

  //created
  if (response.status === 201) {
    const location = response.headers.get("Location");
    if (location) {
      res.location = location;
    }
  }
  return res;
};

async function MapToError(response: Response) {
  const bodyText = await response.text();
  const err = JSON.parse(bodyText);
  return new Error(err.message);
}
