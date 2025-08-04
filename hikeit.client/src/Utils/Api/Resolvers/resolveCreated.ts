export const resolveCreated = async (response: Response) => {
  if (!response.ok) {
    throw new Error();
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
