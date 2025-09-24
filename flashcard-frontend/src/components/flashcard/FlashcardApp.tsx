"use client";

import { useState } from "react";
import Flashcard from "./Flashcard";
import FlashcardControls from "./FlashcardControls";
import FlashcardNavigation from "./FlashcardNavigation";

interface FlashcardData {
  id: number;
  question: string;
  answer: string;
}

const flashcards: FlashcardData[] = [
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
  {
    id: 3,
    question: "Who wrote 'Romeo and Juliet'?",
    answer:
      "William Shakespeare wrote 'Romeo and Juliet' around 1594-1596. It's one of his most famous tragedies.",
  },
  {
    id: 4,
    question: "What is the chemical symbol for gold?",
    answer:
      "Au is the chemical symbol for gold, derived from the Latin word 'aurum' meaning gold.",
  },
  {
    id: 5,
    question: "In what year did World War II end?",
    answer:
      "World War II ended in 1945, with Germany surrendering in May and Japan surrendering in September.",
  },
];

export default function FlashcardApp() {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isFlipped, setIsFlipped] = useState(false);

  const currentCard = flashcards[currentIndex];

  const handleFlip = () => {
    setIsFlipped(!isFlipped);
  };

  const handleReset = () => {
    setIsFlipped(false);
  };

  const handleNext = () => {
    if (currentIndex < flashcards.length - 1) {
      setCurrentIndex(currentIndex + 1);
      setIsFlipped(false);
    }
  };

  const handlePrevious = () => {
    if (currentIndex > 0) {
      setCurrentIndex(currentIndex - 1);
      setIsFlipped(false);
    }
  };

  const handleGoToCard = (index: number) => {
    setCurrentIndex(index);
    setIsFlipped(false);
  };

  return (
    <div className="w-full max-w-2xl mx-auto space-y-8">
      {/* Card Counter */}
      <div className="text-center">
        <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-secondary text-secondary-foreground">
          {currentIndex + 1} / {flashcards.length}
        </span>
      </div>

      {/* Flashcard */}
      <Flashcard card={currentCard} isFlipped={isFlipped} onFlip={handleFlip} />

      {/* Action Buttons */}
      <FlashcardControls
        isFlipped={isFlipped}
        onFlip={handleFlip}
        onReset={handleReset}
      />

      {/* Navigation */}
      <FlashcardNavigation
        currentIndex={currentIndex}
        totalCards={flashcards.length}
        onPrevious={handlePrevious}
        onNext={handleNext}
        onGoToCard={handleGoToCard}
      />
    </div>
  );
}
