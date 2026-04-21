import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getData } from "./auth-test.service";

/**
 * Checks if the provided type exists in the list of valid types.
 * @param type the type to check
 * @returns the valid type if it exists, otherwise 'no_available_type'
 */
const checkIfTypeExists = (type: string): string => {
  const validTypes = ["checkGroup", "apptesters", "test", "admin"];

  if (validTypes.includes(type)) {
    return type;
  } else {
    return "no_available_type";
  }
};

export default function AuthTestComponent() {
  const { type } = useParams<{ type: string }>();
  const [message, setMessage] = useState("Loading...");

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const validType = checkIfTypeExists(type || "");

        const response = await getData(validType);
        setMessage(response?.message ?? "No message");
      } catch (error) {
        if (error instanceof Error) {
          setMessage(`Error: ${error.message}`);
        } else {
          setMessage("Error fetching user");
        }
      }
    };

    fetchUser();
  }, [type]); // Reaguje na zmianę parametru type

  return <p>{message}</p>;
}
