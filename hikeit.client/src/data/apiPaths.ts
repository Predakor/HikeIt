const config = {
  originPath: import.meta.env.VITE_API_ORIGIN,
  apiPrefix: import.meta.env.VITE_API_PREFIX,
};

export const apiPath = `${config.originPath}/${config.apiPrefix}/`;
