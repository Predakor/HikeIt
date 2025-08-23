import { routes } from "@/data/routes/routes";
import { Route, Routes } from "react-router";

function RenderRoutes() {
  return (
    <Routes>
      {routes.map((entry) => {
        if (entry.type === "group") {
          return (
            <Route path={entry.path}>
              {entry.pages.map(({ Page, path }) => (
                <Route path={path} element={<Page />} />
              ))}
            </Route>
          );
        }
        return (
          <Route path={entry.path} element={<entry.Page />} key={entry.path} />
        );
      })}
    </Routes>
  );
}
export default RenderRoutes;
