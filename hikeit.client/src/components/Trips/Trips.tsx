import useFetch from "@/hooks/useFetch";

interface Trip {
  id: number;
  height: number; // Required float
  length: number; // Required float
  duration: number; // Required float
  tripDay?: string; // DateOnly will be represented as a string in TypeScript (ISO format)

  regionID: number; // Required int
}

interface Region {
  id: number;
  name: string;
}

function Trips() {
  const { data, error, loading } = useFetch<Trip[]>("trips");

  if (loading) {
    return <p>Wait a momment api is thinking</p>;
  }

  if (error) {
    return <p>Hmmm looks like api is not aping</p>;
  }

  if (!data) {
    return <p>No data ;c</p>;
  }

  return (
    <div>
      {data.map((trip) => {
        return (
          <div>
            <h2>{trip.length}</h2>
            <p>{trip.height}</p>
            <p>{trip.duration}</p>
          </div>
        );
      })}
    </div>
  );
}

export default Trips;
