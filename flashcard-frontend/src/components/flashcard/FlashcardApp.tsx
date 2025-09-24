"use client";

import Flashcard from "./Flashcard";
import FlashcardControls from "./FlashcardControls";
import FlashcardNavigation from "./FlashcardNavigation";
import { useFlashcards } from "@/hooks/useFlashcards";

export default function FlashcardApp() {
  const {
    currentCard,
    currentIndex,
    isFlipped,
    isLoading,
    error,
    flashcards,
    flipCard,
    resetCard,
    nextCard,
    previousCard,
    goToCard,
    fetchRandomFlashcard,
  } = useFlashcards();

  if (isLoading && flashcards.length === 0) {
    return (
      <div className="w-full max-w-2xl mx-auto space-y-8">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto"></div>
          <p className="mt-4 text-muted-foreground">Loading flashcards...</p>
        </div>
      </div>
    );
  }

  if (error && flashcards.length === 0) {
    return (
      <div className="w-full max-w-2xl mx-auto space-y-8">
        <div className="text-center">
          <p className="text-destructive">Error: {error}</p>
        </div>
      </div>
    );
  }

  if (!currentCard) {
    return (
      <div className="w-full max-w-2xl mx-auto space-y-8">
        <div className="text-center">
          <p className="text-muted-foreground">No flashcards available</p>
        </div>
      </div>
    );
  }

  return (
    <div className="w-full max-w-2xl mx-auto space-y-8">
      {/* Error Message */}
      {error && (
        <div className="text-center">
          <p className="text-sm text-destructive bg-destructive/10 p-2 rounded">
            {error}
          </p>
        </div>
      )}

      {/* Card Counter */}
      <div className="text-center">
        <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-secondary text-secondary-foreground">
          {currentIndex + 1} / {flashcards.length}
        </span>
      </div>

      {/* Flashcard */}
      <Flashcard card={currentCard} isFlipped={isFlipped} onFlip={flipCard} />

      {/* Action Buttons */}
      <FlashcardControls
        isFlipped={isFlipped}
        onFlip={flipCard}
        onReset={resetCard}
        onRandom={fetchRandomFlashcard}
        isLoading={isLoading}
      />

      {/* Navigation */}
      <FlashcardNavigation
        currentIndex={currentIndex}
        totalCards={flashcards.length}
        onPrevious={previousCard}
        onNext={nextCard}
        onGoToCard={goToCard}
      />
    </div>
  );
}
