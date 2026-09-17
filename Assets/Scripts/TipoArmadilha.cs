using UnityEngine;

public enum ModoArmadilha
{
    Espetos,
    Torreta
}

public class TipoArmadilha : MonoBehaviour
{
    [Header("Configuração Geral")]
    [SerializeField] private ModoArmadilha tipo = ModoArmadilha.Espetos;
    private Animator animator;

    [SerializeField] private float atrasoInicial = 1f;

    [Header(" Configurações dos Espetos")]
    [SerializeField] private Collider2D colisorDano;
    [SerializeField] private float tempoAtivo = 2f;
    [SerializeField] private float tempoInativo = 1.5f;

    [Header(" Configurações da Torreta")]
    [SerializeField] private float tempoEntreDisparos = 3f;
    [SerializeField] private GameObject prefabProjetil;
    [SerializeField] private Transform pontoDisparo;
    [SerializeField] private float velocidadeProjetil = 8f;

    private float cronometro;
    private bool emAtraso = true;

    private int estadoEspetos = 1;
    private int estadoTorreta = 0;

    private void Start()
    {
        // Pega o Animator do próprio objeto se não for configurado
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Pega o Collider2D do próprio objeto se não for configurado
        if (colisorDano == null)
        {
            colisorDano = GetComponent<Collider2D>();
        }

        // Começa com o dano desativado
        if (colisorDano != null)
        {
            colisorDano.enabled = false;
        }

        // Começa no estado inicial da animação
        if (animator != null)
        {
            animator.SetInteger("estado", 0);
        }

        cronometro = 0f;
        emAtraso = true;
    }

    private void Update()
    {
        // Atraso inicial para qualquer tipo de armadilha
        if (emAtraso)
        {
            cronometro += Time.deltaTime;

            if (cronometro >= atrasoInicial)
            {
                emAtraso = false;
                cronometro = 0f;
            }

            return;
        }

        // Escolhe o comportamento de acordo com o tipo
        switch (tipo)
        {
            case ModoArmadilha.Espetos:
                AtualizarEspetos();
                break;

            case ModoArmadilha.Torreta:
                AtualizarTorreta();
                break;
        }
    }

    private void AtualizarEspetos()
    {
        cronometro += Time.deltaTime;

        switch (estadoEspetos)
        {
            // Estado 1 = Espetos subindo
            case 1:

                if (animator != null)
                {
                    animator.SetInteger("estado", 1);
                }

                if (colisorDano != null)
                {
                    colisorDano.enabled = true;
                }

                estadoEspetos = 2;
                cronometro = 0f;

                break;

            // Estado 2 = Espetos expostos
            case 2:

                if (animator != null)
                {
                    animator.SetInteger("estado", 2);
                }

                if (colisorDano != null)
                {
                    colisorDano.enabled = true;
                }

                if (cronometro >= tempoAtivo)
                {
                    estadoEspetos = 3;
                    cronometro = 0f;
                }

                break;

            // Estado 3 = Espetos recolhendo
            case 3:

                if (animator != null)
                {
                    animator.SetInteger("estado", 3);
                }

                if (colisorDano != null)
                {
                    colisorDano.enabled = false;
                }

                if (cronometro >= tempoInativo)
                {
                    estadoEspetos = 1;
                    cronometro = 0f;
                }

                break;
        }
    }

    private void AtualizarTorreta()
    {
        cronometro += Time.deltaTime;

        switch (estadoTorreta)
        {
            // Estado 0 = Idle
            case 0:

                if (animator != null)
                {
                    animator.SetInteger("estado", 0);
                }

                if (cronometro >= tempoEntreDisparos)
                {
                    estadoTorreta = 1;
                    cronometro = 0f;
                }

                break;

            // Estado 1 = Atirar
            case 1:

                if (animator != null)
                {
                    animator.SetInteger("estado", 1);
                }

               

                estadoTorreta = 0;
                cronometro = 0f;

                break;
        }
    }

    public void DispararProjetil()
    {
        if (prefabProjetil == null)
        {
            Debug.LogWarning("O prefab do projétil não foi configurado.");
            return;
        }

        if (pontoDisparo == null)
        {
            Debug.LogWarning("O ponto de disparo não foi configurado.");
            return;
        }

        // Cria o projétil no ponto de disparo
        GameObject projetil = Instantiate(
            prefabProjetil,
            pontoDisparo.position,
            pontoDisparo.rotation
        );


    }
}
