import { useEffect } from "react";
import { useFlashcardStore } from "@/stores/flashcardStore";

const API_BASE_URL =
  process.env.NEXT_PUBLIC_API_URL || "http://localhost:7071/api";

export const useFlashcards = () => {
  const {
    flashcards,
    currentIndex,
    isFlipped,
    isLoading,
    error,
    setFlashcards,
    setCurrentIndex,
    setIsFlipped,
    setLoading,
    setError,
    addFlashcard,
    resetCard,
    nextCard,
    previousCard,
    goToCard,
  } = useFlashcardStore();

  const currentCard = flashcards[currentIndex];

  // API functions
  const fetchAllFlashcards = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await fetch(`${API_BASE_URL}/flashcards`);
      if (!response.ok) {
        throw new Error("Failed to fetch flashcards");
      }
      const data = await response.json();
      setFlashcards(data);
    } catch (err) {
      const errorMessage =
        err instanceof Error ? err.message : "Failed to load flashcards";
      setError(errorMessage);
      // Fallback to local data
      setFlashcards([
        {
          id: 1,
          question: "What is the capital of France?",
          answer:
            "Paris is the capital and most populous city of France, known for its art, fashion, gastronomy, and culture.",
        },
        {
          id: 2,
          question: "What is the largest planet in our solar system?",
          answer:
            "Jupiter is the largest planet in our solar system, with a mass more than twice that of all other planets combined.",
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const fetchRandomFlashcard = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await fetch(`${API_BASE_URL}/flashcards/random`);
      if (!response.ok) {
        throw new Error("Failed to fetch random flashcard");
      }
      const randomCard = await response.json();

      // Check if card already exists
      const existingIndex = flashcards.findIndex(
        (card) => card.id === randomCard.id
      );

      if (existingIndex !== -1) {
        goToCard(existingIndex);
      } else {
        addFlashcard(randomCard);
        goToCard(flashcards.length);
      }
    } catch (err) {
      const errorMessage =
        err instanceof Error ? err.message : "Failed to load random card";
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  const flipCard = () => {
    setIsFlipped(!isFlipped);
  };

  // Load flashcards on mount
  useEffect(() => {
    if (flashcards.length === 0) {
      fetchAllFlashcards();
    }
  }, []);

  return {
    // State
    flashcards,
    currentCard,
    currentIndex,
    isFlipped,
    isLoading,
    error,

    // Actions
    flipCard,
    resetCard,
    nextCard,
    previousCard,
    goToCard,
    fetchRandomFlashcard,
    fetchAllFlashcards,
  };
};
