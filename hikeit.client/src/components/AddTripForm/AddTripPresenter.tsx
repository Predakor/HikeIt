import { Field, Input, InputGroup, Stack } from "@chakra-ui/react";
import { type UseFormReturn } from "react-hook-form";
import RegionSelect from "../RegionSelect/RegionSelect";
import { tripFormConfig } from "./AddTrip/data";
import type { TripDto } from "./AddTrip/types";

interface Props {
  formHandler: UseFormReturn<TripDto>;
}

function AddTripPresenter({ formHandler }: Props) {
  const { register, setValue } = formHandler;

  return (
    <Stack>
      {tripFormConfig.map((entry) => (
        <Field.Root key={entry.name}>
          <Field.Label>{entry.label}</Field.Label>
          <InputGroup endElement={entry.unit}>
            <Input
              type={entry.type}
              {...register(entry.name as keyof TripDto)}
            />
          </InputGroup>
        </Field.Root>
      ))}

      <Field.Root>
        <Field.Label>Region</Field.Label>
        <RegionSelect
          onValueChange={(region) => setValue("regionId", region.id)}
        />
      </Field.Root>
    </Stack>
  );
}

// function TripChart() {
//   const [chartData, setChartData] = useState<GpxArray | null>(null);

//   const handleFileMapping = async (file: File) => {
//     const builder = await GpxArrayBuilder.fromFile(file);
//     const gpxArray = builder.smoothMedian(5).generateGains().build();

//     const stats = builder.getStats();
//     console.log({ stats, gpxArray });

//     const chartGpxArray = builder.downsample(500).smoothMedian(10).build();

//     setChartData(chartGpxArray);
//   };

//   if (chartData) {
//     return <TripChart data={chartData} />;
//   }
// }

export default AddTripPresenter;
