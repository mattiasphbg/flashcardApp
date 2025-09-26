import { create } from "zustand";

export interface FlashcardData {
  id: number;
  question: string;
  answer: string;
}

interface FlashcardState {
  flashcards: FlashcardData[];
  currentIndex: number;
  isFlipped: boolean;
  isLoading: boolean;
  error: string | null;

  // Actions
  setFlashcards: (flashcards: FlashcardData[]) => void;
  setCurrentIndex: (index: number) => void;
  setIsFlipped: (flipped: boolean) => void;
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  addFlashcard: (flashcard: FlashcardData) => void;
  resetCard: () => void;
  nextCard: () => void;
  previousCard: () => void;
  goToCard: (index: number) => void;
}

export const useFlashcardStore = create<FlashcardState>((set) => ({
  flashcards: [],
  currentIndex: 0,
  isFlipped: false,
  isLoading: false,
  error: null,

  setFlashcards: (flashcards) => set({ flashcards }),
  setCurrentIndex: (index) => set({ currentIndex: index }),
  setIsFlipped: (flipped) => set({ isFlipped: flipped }),
  setLoading: (loading) => set({ isLoading: loading }),
  setError: (error) => set({ error }),

  addFlashcard: (flashcard) =>
    set((state) => ({
      flashcards: [...state.flashcards, flashcard],
    })),

  resetCard: () => set({ isFlipped: false }),

  nextCard: () =>
    set((state) => {
      const nextIndex = state.currentIndex + 1;
      return {
        currentIndex: nextIndex,
        isFlipped: false,
      };
    }),

  previousCard: () =>
    set((state) => {
      const prevIndex = Math.max(state.currentIndex - 1, 0);
      return {
        currentIndex: prevIndex,
        isFlipped: false,
      };
    }),

  goToCard: (index) =>
    set({
      currentIndex: index,
      isFlipped: false,
    }),
}));
