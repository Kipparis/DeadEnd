using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Создать отдельный скрипт для каждой буквы с разным тайм старт и переходами

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;   // TODO: Сделать поля для более быстрого обращения

    public Animator animator;

    // Переменные для содержания всех предложений
    private Queue<string> sentences;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}

    public void StartDialogue( Dialogue dialogue) {
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        // Очищаем предложения на всякий случай
        sentences.Clear();

        // Добавляем предложения из диалогов в очередь
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence); 
        }

        // Отображаем первое\следующее предложение
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) { // Мы достигли конца диалога
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();  // Берём первое предложение из очереди
        dialogueText.text = sentence;
        StopAllCoroutines();    // Если предложение уже начало печататься, оно прервёт его выполнение
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence) {
        dialogueText.text = "";
        foreach (char c in sentence) {
            dialogueText.text += c;
            yield return null;
        }
    }

    void EndDialogue() {
        Debug.Log("End of conversation.");

        animator.SetBool("isOpen", false);
        // Герой больше не разговаривает
        Hero.S.isTalking = false;
    }
}
