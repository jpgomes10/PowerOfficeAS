const API_BASE_URL = process.env.REACT_APP_BASE_URL;

async function postOrganizations(formData) {
  try {
    const response = await fetch(`${API_BASE_URL}/organization/`, {
      method: "POST",
      body: formData,
    });
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

async function postOrganizationStats(formData) {
  try {
    const response = await fetch(`${API_BASE_URL}/organization/stats`, {
      method: "POST",
      body: formData,
    });
    const data = await response.json();
    return data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export { postOrganizations, postOrganizationStats };
