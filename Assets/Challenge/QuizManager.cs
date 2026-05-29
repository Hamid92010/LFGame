using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class QuizManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text tituloTexto;
    public TMP_Text preguntaTexto;
    public TMP_Text resultadoTexto;
    public TMP_Text kilometrosTexto;
    public TMP_Text embestidaTexto;

    [Header("Fade")]
    public Image fadeImage;

    [Header("Preguntas")]
    public List<Pregunta> preguntas = new List<Pregunta>();

    [Header("Progreso")]
    public int kilometrosPorPregunta = 2;

    private int preguntaActual = 0;
    private int errores = 0;
    private int kilometrosAvanzados = 0;
    private bool esperandoRespuesta = false;

    private void Start()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            fadeImage.color = new Color(color.r, color.g, color.b, 0f);
        }

        ActualizarKilometrosTexto();
        ActualizarEmbestidaTexto();

        MostrarPregunta();
    }

    private void Update()
    {
        if (esperandoRespuesta)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            VerificarRespuesta(true);
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            VerificarRespuesta(false);
        }
    }

    private void MostrarPregunta()
    {
        if (preguntaActual >= preguntas.Count)
        {
            tituloTexto.text = "¡Ganaste!";
            preguntaTexto.text = "";
            resultadoTexto.text = "";

            StartCoroutine(FadeYRegresarMenu());
            return;
        }

        tituloTexto.text = "Pregunta " + (preguntaActual + 1);
        preguntaTexto.text = preguntas[preguntaActual].textoPregunta;

        resultadoTexto.text = "";
        resultadoTexto.color = Color.white;

        esperandoRespuesta = false;
    }

    private void VerificarRespuesta(bool respuestaUsuario)
    {
        esperandoRespuesta = true;

        AumentarKilometros();

        bool respuestaCorrecta = preguntas[preguntaActual].respuestaCorrecta;

        if (respuestaUsuario == respuestaCorrecta)
        {
            resultadoTexto.text = "¡Correcto!";
            resultadoTexto.color = Color.green;
        }
        else
        {
            errores++;
            ActualizarEmbestidaTexto();

            resultadoTexto.text = "Incorrecto";
            resultadoTexto.color = Color.red;

            if (errores >= 3)
            {
                StartCoroutine(Perder());
                return;
            }
        }

        StartCoroutine(SiguientePregunta());
    }

    private void AumentarKilometros()
    {
        kilometrosAvanzados += kilometrosPorPregunta;
        ActualizarKilometrosTexto();
    }

    private void ActualizarKilometrosTexto()
    {
        if (kilometrosTexto != null)
        {
            kilometrosTexto.text = kilometrosAvanzados + " km";
        }
    }

    private void ActualizarEmbestidaTexto()
    {
        if (embestidaTexto != null)
        {
            embestidaTexto.text = "Embestida: " + errores;
        }
    }

    private IEnumerator SiguientePregunta()
    {
        yield return new WaitForSeconds(2f);

        preguntaActual++;

        MostrarPregunta();
    }

    private IEnumerator Perder()
    {
        tituloTexto.text = "Has perdido";
        preguntaTexto.text = "";

        resultadoTexto.text = "Cometiste 3 errores";
        resultadoTexto.color = Color.red;

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeYRegresarMenu());
    }

    private IEnumerator FadeYRegresarMenu()
    {
        float duracionFade = 4f;
        float tiempo = 0f;

        if (fadeImage == null)
        {
            SceneManager.LoadScene("Menu");
            yield break;
        }

        Color color = fadeImage.color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFade);

            fadeImage.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        SceneManager.LoadScene("Menu");
    }
}
