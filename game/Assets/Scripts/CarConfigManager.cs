using UnityEngine;
using System.Runtime.InteropServices;

public class CarConfigManager : MonoBehaviour {
    [SerializeField] Material InitialMaterial = null;

    private string LastRimSelection = "OldStyle";

    // Import the JSLib as following. Make sure the
    // names match with the JSLib file we've just created.
    [DllImport("__Internal")]
    private static extern void ReadyForCommands();

    void Start() {
        ChangeConfiguration("ChangeCar:ElantraF");
        ChangeConfiguration("ChangePaint:CarPaintRed");

        Run.After(0.02f, () => {
            ChangeConfiguration("ChangeRim:MomoRevenge");
        });

        Run.After(0.1f, () => {
            ReadyForCommands();
        });
    }

    public void ChangeConfiguration(string keyValue) {
        string[] parts = keyValue.Split(':');
        string actionName = parts[0];
        string actionValue = parts[1];

        switch (actionName) {
            case "ChangeCar":
                GameObject newCar = (GameObject)Resources.Load("Prefabs/Cars/" + actionValue);

                if (newCar == null) {
                    return;
                }

                // Destroy old cars
                foreach (GameObject car in GameObject.FindGameObjectsWithTag("Car")) {
                    Destroy(car);
                }

                // Instantiate new car
                GameObject newCarInstance = Instantiate(newCar, new Vector3(0, 0, 0), Quaternion.identity);
                newCarInstance.tag = "Car";

                // Keep the same rims
                Run.After(0.01f, () => {
                    ChangeConfiguration("ChangeRim:" + LastRimSelection);
                });
                break;


            case "ChangeRim":
                GameObject newTire = (GameObject)Resources.Load("Prefabs/Rims/" + actionValue);
                LastRimSelection = actionValue;

                if (newTire == null) {
                    return;
                }

                // Destory old tires
                foreach (GameObject tire in GameObject.FindGameObjectsWithTag("Tire")) {
                    Destroy(tire);
                }

                // Get ire locations of current car
                GameObject[] tireParents = new GameObject[]{
                    GameObject.FindGameObjectWithTag("FrontLeftTire"),
                    GameObject.FindGameObjectWithTag("FrontRightTire"),
                    GameObject.FindGameObjectWithTag("RearLeftTire"),
                    GameObject.FindGameObjectWithTag("RearRightTire")
                };

                // Instantiate new tires in the correct locations
                foreach (GameObject tireParent in tireParents) {
                    GameObject newTireInstance = Instantiate(newTire, tireParent.transform.position, Quaternion.identity);
                    if (tireParent.name.Contains("Right")) {
                        newTireInstance.transform.rotation = Quaternion.Euler(
                            newTireInstance.transform.rotation.x,
                            -180,
                            newTireInstance.transform.rotation.z
                        );
                    }
                    newTireInstance.tag = "Tire";
                }

                break;


            case "ChangePaint":
                Material paint = (Material)Resources.Load("Materials/Paints/" + actionValue);

                if (paint == null) {
                    return;
                }

                InitialMaterial.CopyPropertiesFromMaterial(paint);
                break;

            default:
                break;
        }
    }
}
