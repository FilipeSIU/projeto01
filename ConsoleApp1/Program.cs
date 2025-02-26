using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao jogo de Blackjack!");

            int dinheiro = 1000;
            while (dinheiro > 0)
            {
                Console.WriteLine($"Você tem {dinheiro} dólares.");
                Console.Write("Quanto você quer apostar? ");
                int aposta = int.Parse(Console.ReadLine());

                if (aposta > dinheiro)
                {
                    Console.WriteLine("Você não tem dinheiro suficiente para essa aposta.");
                    continue;
                }

                Deck deck = new Deck();
                deck.Shuffle();

                List<Card> playerHand = new List<Card>();
                List<Card> dealerHand = new List<Card>();

                playerHand.Add(deck.DrawCard());
                playerHand.Add(deck.DrawCard());
                dealerHand.Add(deck.DrawCard());
                dealerHand.Add(deck.DrawCard());

                bool playerTurn = true;
                bool doubleDown = false;
                while (playerTurn)
                {
                    Console.WriteLine($"Sua mão: {string.Join(", ", playerHand)} (Total: {GetHandValue(playerHand)})");
                    Console.WriteLine($"Carta visível do dealer: {dealerHand[0]}");

                    if (GetHandValue(playerHand) == 21)
                    {
                        Console.WriteLine("Blackjack! Você ganhou!");
                        dinheiro += aposta;
                        return;
                    }

                    Console.Write("Deseja 'h' para hit, 's' para stand, 'd' para double ou 'p' para split? ");
                    string choice = Console.ReadLine();

                    if (choice == "h")
                    {
                        playerHand.Add(deck.DrawCard());
                        if (GetHandValue(playerHand) > 21)
                        {
                            Console.WriteLine($"Sua mão: {string.Join(", ", playerHand)} (Total: {GetHandValue(playerHand)})");
                            Console.WriteLine("Você estourou! Dealer ganhou.");
                            dinheiro -= aposta;
                            return;
                        }
                    }
                    else if (choice == "s")
                    {
                        playerTurn = false;
                    }
                    else if (choice == "d")
                    {
                        if (aposta * 2 > dinheiro)
                        {
                            Console.WriteLine("Você não tem dinheiro suficiente para dar double.");
                            continue;
                        }
                        aposta *= 2;
                        playerHand.Add(deck.DrawCard());
                        doubleDown = true;
                        playerTurn = false;
                    }
                    else if (choice == "p" && playerHand.Count == 2 && playerHand[0].Rank == playerHand[1].Rank)
                    {
                        List<Card> splitHand1 = new List<Card> { playerHand[0], deck.DrawCard() };
                        List<Card> splitHand2 = new List<Card> { playerHand[1], deck.DrawCard() };

                        Console.WriteLine($"Primeira mão: {string.Join(", ", splitHand1)} (Total: {GetHandValue(splitHand1)})");
                        Console.WriteLine($"Segunda mão: {string.Join(", ", splitHand2)} (Total: {GetHandValue(splitHand2)})");

                        // Jogar a primeira mão
                        JogarMao(deck, splitHand1, dealerHand, ref dinheiro, aposta);

                        // Jogar a segunda mão
                        JogarMao(deck, splitHand2, dealerHand, ref dinheiro, aposta);

                        return;
                    }
                    else
                    {
                        Console.WriteLine("Escolha inválida. Tente novamente.");
                    }
                }

                while (GetHandValue(dealerHand) < 17)
                {
                    dealerHand.Add(deck.DrawCard());
                }

                Console.WriteLine($"Mão do dealer: {string.Join(", ", dealerHand)} (Total: {GetHandValue(dealerHand)})");

                if (GetHandValue(dealerHand) > 21 || GetHandValue(playerHand) > GetHandValue(dealerHand))
                {
                    Console.WriteLine("Você ganhou!");
                    dinheiro += aposta;
                }
                else if (GetHandValue(playerHand) < GetHandValue(dealerHand))
                {
                    Console.WriteLine("Dealer ganhou.");
                    dinheiro -= aposta;
                }
                else
                {
                    Console.WriteLine("Empate.");
                }

                if (doubleDown)
                {
                    break;
                }
            }

            Console.WriteLine("Você ficou sem dinheiro. Fim do jogo.");
        }

        static void JogarMao(Deck deck, List<Card> hand, List<Card> dealerHand, ref int dinheiro, int aposta)
        {
            bool playerTurn = true;
            while (playerTurn)
            {
                Console.WriteLine($"Sua mão: {string.Join(", ", hand)} (Total: {GetHandValue(hand)})");
                Console.WriteLine($"Carta visível do dealer: {dealerHand[0]}");

                if (GetHandValue(hand) == 21)
                {
                    Console.WriteLine("Blackjack! Você ganhou!");
                    dinheiro += aposta;
                    return;
                }

                Console.Write("Deseja 'h' para hit ou 's' para stand? ");
                string choice = Console.ReadLine();

                if (choice == "h")
                {
                    hand.Add(deck.DrawCard());
                    if (GetHandValue(hand) > 21)
                    {
                        Console.WriteLine($"Sua mão: {string.Join(", ", hand)} (Total: {GetHandValue(hand)})");
                        Console.WriteLine("Você estourou! Dealer ganhou.");
                        dinheiro -= aposta;
                        return;
                    }
                }
                else if (choice == "s")
                {
                    playerTurn = false;
                }
                else
                {
                    Console.WriteLine("Escolha inválida. Tente novamente.");
                }
            }

            while (GetHandValue(dealerHand) < 17)
            {
                dealerHand.Add(deck.DrawCard());
            }

            Console.WriteLine($"Mão do dealer: {string.Join(", ", dealerHand)} (Total: {GetHandValue(dealerHand)})");

            if (GetHandValue(dealerHand) > 21 || GetHandValue(hand) > GetHandValue(dealerHand))
            {
                Console.WriteLine("Você ganhou!");
                dinheiro += aposta;
            }
            else if (GetHandValue(hand) < GetHandValue(dealerHand))
            {
                Console.WriteLine("Dealer ganhou.");
                dinheiro -= aposta;
            }
            else
            {
                Console.WriteLine("Empate.");
            }
        }

        static int GetHandValue(List<Card> hand)
        {
            int value = 0;
            int aceCount = 0;

            foreach (var card in hand)
            {
                if (card.Rank == "A")
                {
                    aceCount++;
                    value += 11;
                }
                else if (card.Rank == "K" || card.Rank == "Q" || card.Rank == "J")
                {
                    value += 10;
                }
                else
                {
                    value += int.Parse(card.Rank);
                }
            }

            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }

            return value;
        }
    }

    class Card
    {
        public string Suit { get; }
        public string Rank { get; }

        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public override string ToString()
        {
            return $"{Rank} de {Suit}";
        }
    }

    class Deck
    {
        private List<Card> cards;
        private Random random = new Random();

        public Deck()
        {
            cards = new List<Card>();
            string[] suits = { "Copas", "Ouros", "Espadas", "Paus" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card DrawCard()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }
}
