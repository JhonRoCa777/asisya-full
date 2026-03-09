import { BASE_API } from "../boot/axios";
import { SwalHelper } from "../helpers/Swal";
import { ROUTER } from "../router";

BASE_API.interceptors.response.use(

  async (response) => {

    const apiResponse = response.data;
    let message = ''
    let code = 0;

    if(apiResponse?.error) 
    {
        code = apiResponse.error.code;
        message = apiResponse.error.message;
    }

    if(apiResponse?.Error)
    {
        code = apiResponse.Error.Code;
        message = apiResponse.Error.Message;
    }

    if (code > 0) {

      await SwalHelper.error(message);

      switch (code) {

        case 401:
          setTimeout(() => {
            window.location.href = "/" + ROUTER.HOME.MAIN;
          }, 500);
          break;

        case 403:
          setTimeout(() => {
            window.location.href = "/";
          }, 500);
          break;
      }

      return Promise.reject(apiResponse);
    }

    return response;
  },

  async (error) => {

    const message =
      error?.response?.data?.error?.message ||
      error?.response?.data?.message ||
      "Error inesperado";

    await SwalHelper.error(message);
      return await Promise.reject(error);
  }
);