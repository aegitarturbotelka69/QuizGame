using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quiz.Modules;

namespace Quiz.Quiz
{
    public class UI_ItemCreator : MonoBehaviour
    {
        void Awake()
        {
            UI_Manager.OnSendListOfItems += InstanciateUIComponents;
        }


        private void InstanciateUIComponents(List<Item> listOfItems)
        {
            System.Random rng = new System.Random();
            int rngCorrectAnswerPosition = rng.Next(0, (listOfItems.Count));

            int x = 0;
            foreach (Item item in listOfItems)
            {
                GameObject gameObject = CreateItem(item);

                if (x == rngCorrectAnswerPosition)
                {
                    UI_Manager.Instance.SetCorrectAnswer(item);
                    gameObject.AddComponent<UI_OnCorrectAnswer>();
                }
                else
                {
                    gameObject.AddComponent<UI_OnWrongAnswer>();
                    gameObject.GetComponent<UI_OnWrongAnswer>().ClickCooldown = 1f;
                }

                gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color32((byte)rng.Next(0, 255), (byte)rng.Next(0, 255), (byte)rng.Next(0, 255), 255);
                UI_Manager.Instance.instanciatedUI_GameObjects.Add(gameObject);
                x++;
            }

        }

        private GameObject CreateItem(Item item)
        {
            GameObject itemGameObjectInstance = Instantiate(Resources.Load("Item template"), this.gameObject.transform) as GameObject;

            itemGameObjectInstance.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = item.GetImage();

            UI_ItemAnimations objectAnimations = itemGameObjectInstance.GetComponent<UI_ItemAnimations>();
            objectAnimations.BounceAnimation = new Animations.UI_BounceItemAnimation();
            StartCoroutine(objectAnimations.BounceAnimation.Bounce(itemGameObjectInstance));


            return itemGameObjectInstance;
        }
    }
}