import { BASE_API } from "../../boot/axios";
import type { EmployeeLoginDTO } from "../../models/Employee";

export function AuthService() {

  const CONTROLLER = '/Auth';

  return {
    login: (credentials: EmployeeLoginDTO) => BASE_API.post(`${CONTROLLER}/Login`, credentials),
    verify: () => BASE_API.get(`${CONTROLLER}/Verify`),
    logout: async () => {
      // const RESP = await GlobalErrors<boolean>(() => BASE_API.get(`${CONTROLLER}/logout`));
      // if (RESP) {
      //   dispatcher(resetAuthStore());
      //   ROUTER.LOGIN();
      // }
    }
  }
}
