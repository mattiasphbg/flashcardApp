import { useQuery } from "@tanstack/react-query";
import { useFlashcardStore } from "@/stores/flashcardStore";

const API_BASE_URL =
  process.env.NEXT_PUBLIC_API_URL || "http://localhost:7071/api";

export const useFlashcards = () => {
  const {
    flashcards,
    currentIndex,
    isFlipped,
    setIsFlipped,
    addFlashcard,
    resetCard,
    nextCard,
    previousCard,
    goToCard,
  } = useFlashcardStore();

  const {
    data: fetchedFlashcards = [],
    isLoading,
    error,
    refetch: refetchFlashcards,
  } = useQuery({
    queryKey: ["flashcards"],
    queryFn: async () => {
      const response = await fetch(`${API_BASE_URL}/flashcards`);
      if (!response.ok) {
        throw new Error("Failed to fetch flashcards");
      }
      return response.json();
    },
    retry: 1,
    staleTime: 5 * 60 * 1000,
  });

  // Use the fetched data directly, or fall back to store data
  const displayFlashcards =
    fetchedFlashcards.length > 0 ? fetchedFlashcards : flashcards;

  const currentCard = displayFlashcards[currentIndex];

  // Keep your existing fetchRandomFlashcard function
  const fetchRandomFlashcard = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/flashcards/random`);
      if (!response.ok) {
        throw new Error("Failed to fetch random flashcard");
      }
      const randomCard = await response.json();

      const existingIndex = displayFlashcards.findIndex(
        (card: { id: string }) => card.id === randomCard.id
      );

      if (existingIndex !== -1) {
        goToCard(existingIndex);
      } else {
        addFlashcard(randomCard);
        goToCard(displayFlashcards.length);
      }
    } catch (err) {
      console.error("Failed to load random card:", err);
    }
  };

  const flipCard = () => {
    setIsFlipped(!isFlipped);
  };

  return {
    flashcards: displayFlashcards,
    currentCard,
    currentIndex,
    isFlipped,
    isLoading,
    error: error?.message || null,

    flipCard,
    resetCard,
    nextCard,
    previousCard,
    goToCard,
    fetchRandomFlashcard,
    fetchAllFlashcards: refetchFlashcards,
  };
};
